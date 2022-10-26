import { Injectable } from '@angular/core';
import { ActivatedRoute, Router, UrlTree } from '@angular/router';

import { AdminRoutes } from '@app/features/admin/admin.routes';

import { IdentityProvider } from '../enums/identity-provider.enum';
import { AuthService } from '../services/auth.service';
import { AuthGuard } from './abstract-auth.guard';

@Injectable({
  providedIn: 'root',
})
export abstract class AuthenticationGuard extends AuthGuard {
  public idpHint: IdentityProvider;
  public constructor(
    protected authService: AuthService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    super(authService);

    const routeSnapshot = this.route.snapshot;
    this.idpHint = routeSnapshot.data.idpHint;
  }

  protected handleAccessCheck(
    routeRedirect: string | undefined
  ): (authenticated: boolean) => boolean | UrlTree {
    return (authenticated: boolean): boolean | UrlTree =>
      authenticated
        ? this.idpHint === IdentityProvider.IDIR
          ? this.router.createUrlTree([AdminRoutes.MODULE_PATH])
          : true
        : this.router.createUrlTree([routeRedirect ?? '/'], {
            queryParams:
              this.router.getCurrentNavigation()?.extractedUrl.queryParams,
            queryParamsHandling: 'merge',
          });
  }

  protected handleAccessError(): boolean {
    return false;
  }
}
