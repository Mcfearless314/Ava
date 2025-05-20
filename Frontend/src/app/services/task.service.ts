import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {ProjectTask} from "../models/project-task.model";

@Injectable({providedIn: 'root'})

export class TaskService {
  constructor(private http: HttpClient) {
  }

  getProjectTasksByProject(projectId: string): Observable<ProjectTask[]> {
    return this.http.get<ProjectTask[]>(`/api/ProjectTask/getAllProjectTasksForProject/${projectId}`);
  }

  updateTaskStatus(projectTaskId: string, newStatus: number, projectId: string): Observable<ProjectTask> {
    return this.http.put<ProjectTask>(
      `/api/ProjectTask/updateStatus/${projectTaskId}/${newStatus}/${projectId}`,
      null
    );
  }

  addTask(projectId: string, newTaskTitle: string, newTaskBody: string): Observable<ProjectTask> {
    return this.http.post<ProjectTask>(
      `/api/ProjectTask/create`
      , {
        title: newTaskTitle,
        body: newTaskBody,
        status: 1,
        projectId: projectId,
      }
    )
  }
}
