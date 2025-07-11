import { inject, Injectable } from '@angular/core';
import { AccountService } from './account-service';
import { of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class InitService {
  private accountService = inject(AccountService);

  init() {
    const userString = localStorage.getItem('user');
    if (!userString) return of(null);
    const currentUser = JSON.parse(userString);
    this.accountService.currentUser.set(currentUser); 

    return of(null);
  }
}
