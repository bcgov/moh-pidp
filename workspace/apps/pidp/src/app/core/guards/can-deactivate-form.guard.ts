import { Injectable } from '@angular/core';
import { CanDeactivate, UrlTree } from '@angular/router';

import { Observable } from 'rxjs';

import { IFormPage } from '../classes/abstract-form-page.class';

@Injectable({
  providedIn: 'root',
})
export class CanDeactivateFormGuard implements CanDeactivate<unknown> {
  public canDeactivate(
    component: IFormPage
  ): Observable<boolean | UrlTree> | boolean {
    // Pass control to the component to determine if deactivation
    // can occur, otherwise, allow route request to occur
    return component.canDeactivate ? component.canDeactivate() : true;
  }
}
