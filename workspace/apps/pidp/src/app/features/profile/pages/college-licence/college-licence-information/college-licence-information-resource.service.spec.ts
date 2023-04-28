import { TestBed } from '@angular/core/testing';
import { provideAutoSpy } from 'jest-auto-spies';

import { CollegeLicenceInformationResource } from './college-licence-information-resource.service';
import { ApiHttpClient } from '@app/core/resources/api-http-client.service';
import { ToastService } from '@app/core/services/toast.service';

describe('CollegeLicenceInformationResource', () => {
  let service: CollegeLicenceInformationResource;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideAutoSpy(ApiHttpClient),
        provideAutoSpy(ToastService),
      ]
    });
    service = TestBed.inject(CollegeLicenceInformationResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
