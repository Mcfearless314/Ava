// toast.component.ts
import { Component } from '@angular/core';
import { ToastService } from '../../services/toast.service';
import {NgForOf} from "@angular/common";

@Component({
  selector: 'app-toast',
  template: `
    <div class="toast-container">
      <div class="toast" *ngFor="let toast of toastService.toasts">
        {{ toast.message }}
      </div>
    </div>
  `,
  standalone: true,
  imports: [
    NgForOf
  ],
  styles: [`
    .toast-container {
      position: fixed;
      bottom: 20px;
      right: 20px;
      z-index: 1000;
    }

    .toast {
      background-color: #333;
      color: white;
      padding: 10px 15px;
      border-radius: 4px;
      margin-top: 10px;
      box-shadow: 0 2px 6px rgba(0, 0, 0, 0.2);
      font-size: 14px;
    }
  `]
})
export class ToastComponent {
  constructor(public toastService: ToastService) {}
}
