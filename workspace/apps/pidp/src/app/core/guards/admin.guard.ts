import { Injectable } from '@angular/core';
import { CanActivate, Router, UrlTree } from '@angular/router';

import { Observable } from 'rxjs';

import { Role } from '@app/shared/enums/roles.enum';

import { AuthorizedUserService } from '../services/authorized-user.service';

@Injectable({
  providedIn: 'root',
})
export class AdminGuard implements CanActivate {
  public constructor(
    private router: Router,
    private authorizedUserService: AuthorizedUserService
  ) {}

  // TODO add other guard methods
  // TODO make something generic and move into specific modules
  // TODO abstract permissions guard
  public canActivate():
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    return this.authorizedUserService.roles.includes(Role.USER)
      ? this.router.createUrlTree(['/'])
      : this.authorizedUserService.roles.includes(Role.ADMIN);
  }
}
