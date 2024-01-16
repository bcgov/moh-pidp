import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';
import { PermissionsService } from '@app/modules/permissions/permissions.service';

import { GetSupportComponent } from './get-support.component';

describe('GetSupportComponent', () => {
  let component: GetSupportComponent;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        GetSupportComponent,
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        provideAutoSpy(PermissionsService),
      ],
    });

    component = TestBed.inject(GetSupportComponent);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
