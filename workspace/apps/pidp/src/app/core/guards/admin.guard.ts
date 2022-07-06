import { Injectable } from '@angular/core';
import { CanActivate, Router, UrlTree } from '@angular/router';

import { Observable } from 'rxjs';

import { AuthorizedUserService } from '@app/features/auth/services/authorized-user.service';
import { Role } from '@app/shared/enums/roles.enum';

@Injectable({
  providedIn: 'root',
})
export class AdminGuard implements CanActivate {
  public constructor(
    private router: Router,
    private authorizedUserService: AuthorizedUserService
  ) {}

  public canActivate():
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    return this.authorizedUserService.roles.includes(Role.ADMIN)
      ? true
      : this.router.createUrlTree(['/']);
  }
}
