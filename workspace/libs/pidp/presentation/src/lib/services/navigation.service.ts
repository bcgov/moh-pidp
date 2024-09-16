import { Injectable } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class NavigationService {
  private currentUrl!: string;
  private previousUrl: string;

  public constructor(private router: Router) {
    this.previousUrl = this.router.url;
    router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.currentUrl = this.previousUrl;
        this.previousUrl = event.url;
      }
    });
  }

  public navigateToRoot(): void {
    this.router.navigateByUrl('/');
  }

  public getCurrentUrl() {
    return this.currentUrl;
  }
  public getPreviousUrl() {
    return this.previousUrl;
  }
}
