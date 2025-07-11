import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ToastService {

  constructor() { 
    this.createToastContainer();
  }

  private createToastContainer() { 
    if(!document.getElementById('toast-container')) {
      const container = document.createElement('div');
      container.id = 'toast-container';
      container.className = 'toast toast-bottom toast-end';
      document.body.appendChild(container);
    } 
  }

  private createToastElemenet(message: string, alertclass: string, duration: number = 5000) {
    const toastContainer = document.getElementById('toast-container');
    if(!toastContainer) return; 

    const toast = document.createElement('div');
    toast.classList.add('toast', ...alertclass.split(' '), 'shadow-lg');
    toast.innerHTML = `
      <span>${message}</span>
      <button class="ml-4 btn btn-sm btn-circle btn-ghost"">x</button>
    `;

    toast.querySelector('button')?.addEventListener('click', () => { 
      toastContainer.removeChild(toast);
    });

    toastContainer.append(toast);

    setTimeout(() => {
      if (toastContainer.contains(toast)) {
        toastContainer.removeChild(toast);
      }
    }, duration);
  }

  success(message: string, duration?: number) { 
    this.createToastElemenet(message, 'alert alert-success', duration);
  }

  error(message: string, duration?: number) { 
    this.createToastElemenet(message, 'alert alert-error', duration);
  }

  warning(message: string, duration?: number) { 
    this.createToastElemenet(message, 'alert alert-warning', duration);
  }

  info(message: string, duration?: number) { 
    this.createToastElemenet(message, 'alert alert-info', duration);
  }
}
