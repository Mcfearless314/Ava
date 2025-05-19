import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { TaskService } from '../../services/task.service';
import { DragDropModule,CdkDragDrop,
  moveItemInArray,
  transferArrayItem
} from '@angular/cdk/drag-drop';
import {ProjectTask} from "../../models/project-task.model";


@Component({
  selector: 'app-project-page',
  standalone: true,
  imports: [CommonModule, DragDropModule],
  providers: [TaskService],
  templateUrl: './project-page.component.html',
  styleUrls: ['./project-page.component.css']
})
export class ProjectPageComponent implements OnInit {
  projectId!: string;
  tasks: ProjectTask[] = [];
  columns: { [key: number]: ProjectTask[] } = {
    1: [],
    2: [],
    3: []
  };

  constructor(private route: ActivatedRoute, private taskService: TaskService) {}

  activeColumn: number | null = null;

  setActiveColumn(column: number) {
    this.activeColumn = column;
  }

  clearActiveColumn() {
    this.activeColumn = null;
  }

  ngOnInit() {
    this.projectId = this.route.snapshot.paramMap.get('projectId')!;
    this.loadTasks();
  }

  loadTasks() {
    this.taskService.getProjectTasksByProject(this.projectId).subscribe(projectTasks  => {
      this.columns = { 1: [], 2: [], 3: [] };
      for (let task of projectTasks) {
        this.columns[task.status]?.push(task);
      }
    });
  }

  columnKeys = [1, 2, 3];

  getColumnTitle(status: number): string {
    return {
      1: 'To Do',
      2: 'In Progress',
      3: 'Done'
    }[status] || 'Unknown';
  }

  drop(event: CdkDragDrop<ProjectTask[]>) {
    const task: ProjectTask = event.item.data;
    const newStatus = Number(event.container.id);


    if (!task || isNaN(newStatus)) {
      console.error('Invalid drop event data');
      return;
    }

    if (task.status !== newStatus) {
      transferArrayItem(
        event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex
      );

      task.status = newStatus;

      this.taskService.updateTaskStatus(task.id, newStatus, task.projectId).subscribe({
        next: () => console.log('Task status updated'),
        error: err => {
          console.error('Failed to update task status', err);
        }
      });
    } else {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    }
  }
}
