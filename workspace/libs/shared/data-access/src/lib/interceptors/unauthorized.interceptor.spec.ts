/* eslint-disable @typescript-eslint/no-explicit-any */
import { HttpClient, HttpStatusCode } from '@angular/common/http';
import {
  HttpClientTestingModule,
  HttpTestingController,
} from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { Router, RouterModule } from '@angular/router';

import { provideAutoSpy } from 'jest-auto-spies';

import { unauthorizedInterceptorProvider } from './unauthorized.interceptor';

describe('UnauthorizedInterceptor', () => {
  let httpClient: HttpClient;
  let httpMock: HttpTestingController;
  let router: Router;

  const mockEndpoint = '/path/of/endpoint/test';
  const redirectUrl = '/';

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, RouterModule.forRoot([])],
      providers: [unauthorizedInterceptorProvider, provideAutoSpy(Router)],
    });

    router = TestBed.inject(Router);
    httpClient = TestBed.inject(HttpClient);
    httpMock = TestBed.inject<any>(HttpTestingController);
  });

  describe('METHOD: intercept', () => {
    given('an HTTP request', () => {
      const request = httpClient.get(mockEndpoint);

      when('the request is successful', () => {
        request.subscribe();
        const httpRequest = httpMock.expectOne(mockEndpoint);

        then('no routing should occur', () => {
          expect(httpRequest.request.url).toBe(mockEndpoint);
          expect(router.navigate).not.toHaveBeenCalledWith([redirectUrl]);
        });
      });
    });

    given('an HTTP request', () => {
      const request = httpClient.get(mockEndpoint);

      when('the request fails', () => {
        request.subscribe();
        const httpRequest = httpMock.expectOne(mockEndpoint);
        httpRequest.error(new ErrorEvent('Simulated Error Event'), {
          status: HttpStatusCode.Unauthorized,
        });

        then('routing should occur', () => {
          expect(httpRequest.request.url).toBe(mockEndpoint);
          expect(router.navigate).toHaveBeenCalledWith([redirectUrl]);
        });
      });
    });
  });
});
