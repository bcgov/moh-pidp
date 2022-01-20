import { Injectable } from '@angular/core';
import { CanActivate, Router, UrlTree } from '@angular/router';

import { Observable } from 'rxjs';

import { Role } from '@app/shared/enums/roles.enum';

import { AuthorizedUserService } from '../services/authorized-user.service';

@Injectable({
  providedIn: 'root',
})
export class UserGuard implements CanActivate {
  public constructor(
    private router: Router,
    private authorizedUserService: AuthorizedUserService
  ) {}

  // TODO add other guard methods
  // TODO make something generic and move into specific modules
  public canActivate():
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    return this.authorizedUserService.roles.includes(Role.ADMIN)
      ? this.router.createUrlTree(['admin'])
      : this.authorizedUserService.roles.includes(Role.USER);
  }
}
