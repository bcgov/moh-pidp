import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { MatSnackBarModule } from '@angular/material/snack-bar';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';

import { AdministratorInformationResource } from './administrator-information-resource.service';

describe('AdministratorInformationResource', () => {
  let service: AdministratorInformationResource;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, MatSnackBarModule],
      providers: [
        AdministratorInformationResource,
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
      ],
    });

    service = TestBed.inject(AdministratorInformationResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
