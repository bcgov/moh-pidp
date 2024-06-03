import { HttpClient } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';
import { KeycloakService } from 'keycloak-angular';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';

import { AuthorizedUserService } from '../../services/authorized-user.service';
import { LinkAccountConfirmPage } from './link-account-confirm.page';

describe('LinkAccountConfirmPage', () => {
  let component: LinkAccountConfirmPage;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [LinkAccountConfirmPage],
      providers: [
        LinkAccountConfirmPage,
        provideAutoSpy(HttpClient),
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        provideAutoSpy(AuthorizedUserService),
        provideAutoSpy(KeycloakService),
      ],
    });

    component = TestBed.inject(LinkAccountConfirmPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
