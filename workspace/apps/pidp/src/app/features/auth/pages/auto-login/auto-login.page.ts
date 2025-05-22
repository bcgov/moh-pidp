import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { APP_CONFIG, AppConfig } from '@app/app.config';

import { AuthRoutes } from '../../auth.routes';
import { IdentityProvider } from '../../enums/identity-provider.enum';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-auto-login',
  templateUrl: './auto-login.page.html',
  standalone: true,
})
export class AutoLoginPage implements OnInit {
  public constructor(
    @Inject(APP_CONFIG) private readonly config: AppConfig,
    private readonly authService: AuthService,
    private readonly route: ActivatedRoute,
    private readonly router: Router,
  ) { }

  public ngOnInit(): void {
    const idpHint = this.route.snapshot.queryParamMap.get('idp_hint') ?? '';

    if (Object.values<string>(IdentityProvider).includes(idpHint)) {
      this.authService
        .login({
          idpHint: idpHint,
          redirectUri: this.config.applicationUrl,
          prompt: 'login'
        })
        .subscribe();
    } else {
      this.router.navigate([AuthRoutes.routePath(AuthRoutes.PORTAL_LOGIN)]);
    }
  }
}
