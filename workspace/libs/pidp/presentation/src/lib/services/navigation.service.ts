import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class NavigationService {
  public constructor(private router: Router) {}
  public navigateToRoot(): void {
    this.router.navigateByUrl('/');
  }
}
