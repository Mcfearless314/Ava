import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {CommonModule} from '@angular/common';
import {TaskService} from '../../services/task.service';
import {
  DragDropModule, CdkDragDrop,
  moveItemInArray,
  transferArrayItem
} from '@angular/cdk/drag-drop';
import {ProjectTask} from "../../models/project-task.model";
import {ToastService} from "../../services/toast.service";
import {FormsModule} from "@angular/forms";
import {User} from "../../models/user.model";
import {UserService} from "../../services/user.service";
import {OrganisationService} from "../../services/organisation.service";
import {ProjectService} from "../../services/project.service";


@Component({
  selector: 'app-project-page',
  standalone: true,
  imports: [CommonModule, DragDropModule, FormsModule],
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

  constructor(
    private route: ActivatedRoute,
    private taskService: TaskService,
    private toastService: ToastService,
    private userService: UserService,
    private organisationService: OrganisationService,
    private projectService: ProjectService){
  }


  ngOnInit() {
    this.projectId = this.route.snapshot.paramMap.get('projectId')!;
    this.loadTasks();
    this.fetchUsersForProject();
    this.fetchOrganisationUsers();
  }

  loadTasks() {
    this.taskService.getProjectTasksByProject(this.projectId).subscribe(projectTasks => {
      this.columns = {1: [], 2: [], 3: []};
      for (let task of projectTasks) {
        this.columns[task.status]?.push(task);
      }
    });
  }

  columnKeys = [1, 2, 3];
  newTaskTitle: string = '';
  newTaskBody: string = '';
  users: User[] = [];
  usersInOrganisation: User[] = [];
  availableUsers: User[] = [];
  selectedUser: string = '';

  getColumnTitle(status
                 :
                 number
  ):
    string {
    return {
      1: 'To Do',
      2: 'In Progress',
      3: 'Done'
    }[status] || 'Unknown';
  }

  drop(event
       :
       CdkDragDrop<ProjectTask[]>
  ) {
    const task: ProjectTask = event.item.data;
    const newProjectTaskStatus = Number(event.container.id);


    if (!task || isNaN(newProjectTaskStatus)) {
      console.error('Invalid drop event data');
      return;
    }

    if (task.status !== newProjectTaskStatus) {
      transferArrayItem(
        event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex
      );

      task.status = newProjectTaskStatus;

      this.taskService.updateTaskStatus(task.id, newProjectTaskStatus, task.projectId).subscribe({
        next: () => this.toastService.show('Task status updated successfully'),
        error: err => {
          this.toastService.show("Failed to update Task Status with reason: " + err);
        }
      });
    } else {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    }
  }

  addTask() {
    this.taskService.addTask(this.projectId, this.newTaskTitle, this.newTaskBody).subscribe({
      next: (projectTask: ProjectTask) => {
        this.toastService.show('Task added successfully');
        this.columns[projectTask.status].push(projectTask);
        this.newTaskTitle = '';
        this.newTaskBody = '';
      }, error: err => {
        if (err.status === 403) {
          this.toastService.show("You do not have permission to add a task to this project.");
        }
        this.toastService.show("Failed to add Task with reason: " + err.error.error);
      }
    });
  }

  fetchUsersForProject() {
    this.userService.getUsersForProject(this.projectId).subscribe({
      next: (users: User[]) => {
        this.users = users;
        this.updateAvailableUsers();
      },
      error: err => {
        this.toastService.show("Failed to fetch users for project with reason: " + err.error.error);
      }
    });
  }

  fetchOrganisationUsers() {
    this.organisationService.getOrganisationId(this.projectId).subscribe(organisationId => {

      this.organisationService.getUsers(organisationId).subscribe({
        next: (users: User[]) => {
          this.usersInOrganisation = users;
          this.updateAvailableUsers();
        },
        error: err => {
          this.toastService.show("Failed to fetch users for organisation with reason: " + err.error.error);
        }
      });
    });
  }

  updateAvailableUsers() {
    const projectUserIds = new Set(this.users.map(u => u.id));
    this.availableUsers = this.usersInOrganisation.filter(u => !projectUserIds.has(u.id));
  }

  addUser() {
    if (this.selectedUser) {
      this.projectService.addUserToProject(this.projectId, this.selectedUser).subscribe({
        next: response => {
          this.toastService.show('User added to project successfully');
          this.users.push(response);
          this.availableUsers = this.availableUsers.filter(user => user.id !== response.id);
          this.selectedUser = '';
        },
        error: err => {
          if (err.status === 403) {
            this.toastService.show("You do not have permission to add a user to this project.");
          } else if (err.status === 404) {
            this.toastService.show("User not found.");
          } else {
            this.toastService.show("Failed to add user with reason: " + err.error.error);
          }
        }
      });
    }
  }


}
