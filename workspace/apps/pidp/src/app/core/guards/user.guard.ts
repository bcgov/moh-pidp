import { Inject, Injectable } from '@angular/core';
import { CanActivate, Router, UrlTree } from '@angular/router';

import { Observable } from 'rxjs';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import { AuthorizedUserService } from '@app/features/auth/services/authorized-user.service';
import { Role } from '@app/shared/enums/roles.enum';

@Injectable({
  providedIn: 'root',
})
export class UserGuard implements CanActivate {
  public constructor(
    @Inject(APP_CONFIG) private config: AppConfig,
    private router: Router,
    private authorizedUserService: AuthorizedUserService
  ) {}

  public canActivate():
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    return this.authorizedUserService.roles.includes(Role.ADMIN)
      ? this.router.createUrlTree([this.config.routes.admin])
      : true;
  }
}
