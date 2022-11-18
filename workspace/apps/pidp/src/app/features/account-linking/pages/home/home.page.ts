import { Component, Inject } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

import { KeycloakService } from 'keycloak-angular';

import { APP_CONFIG, AppConfig } from '@app/app.config';
import { KeycloakInitService } from '@app/modules/keycloak/keycloak-init.service';

@Component({
  selector: 'app-account-linking-home-page',
  templateUrl: './home.page.html',
  styleUrls: ['./home.page.scss'],
})
export class AccountLinkingHomePage {
  public messages: string[] = [];
  public constructor(
    private snackBar: MatSnackBar,
    @Inject(APP_CONFIG) private config: AppConfig,
    private initService: KeycloakInitService,
    private keycloakService: KeycloakService
  ) {}
  // public async load(): Promise<void> {
  //   const authenticated = await this.keycloakService.init(
  //     this.getKeycloakOptions()
  //   );

  //   this.keycloakService.getKeycloakInstance().onTokenExpired = (): void => {
  //     this.keycloakService
  //       .updateToken()
  //       .catch(() => this.router.navigateByUrl(AuthRoutes.MODULE_PATH));
  //   };

  //   if (authenticated) {
  //     // Force refresh to begin expiry timer
  //     await this.keycloakService.updateToken(-1);
  //   }
  // }

  // private getKeycloakOptions(): KeycloakOptions {
  //   return this.config.keycloakConfig;
  // }

  public onLinkClick(): void {
    this.writeMessage('Starting to do something');

    // this.initService.load().then(() => {
    this.keycloakService
      .getToken()
      .then((token) => {
        this.writeMessage(`Got token: ${token}`);
      })
      .catch((error) => {
        this.writeMessage(`token error: ${error}`);
        console.log(error);
      });
    // });

    this.writeMessage('Finished doing something');
    this.snackBar.open('Just kidding!', 'OK');
  }
  public onLink2Click(): void {
    this.writeMessage('Try init');
    this.keycloakService
      .init(this.config.keycloakConfig)
      .then((x) => {
        this.writeMessage('Init sucess');
      })
      .catch((err) => {
        this.writeMessage(`error: ${err}`);
      });
  }
  private writeMessage(s: string): void {
    this.messages.push(s);
  }
}
