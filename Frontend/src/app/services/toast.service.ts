// toast.service.ts
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class ToastService {
  toasts: { message: string }[] = [];

  show(message: string) {
    this.toasts.push({ message });
    setTimeout(() => this.toasts.shift(), 3000);
  }
}
