import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { ApiHttpClient } from '@app/core/resources/api-http-client.service';

import { LoginResource } from './login-resource.service';

describe('LoginResource', () => {
  let service: LoginResource;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideAutoSpy(ApiHttpClient)],
    });

    service = TestBed.inject(LoginResource);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
