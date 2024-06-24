import { TestBed } from '@angular/core/testing';
import { ResolveFn } from '@angular/router';

import { StatusCode } from '@app/features/portal/enums/status-code.enum';

import { provincialAttachmentSystemResolver } from './provincial-attachment-system.resolver';

describe('provincialAttachmentSystemResolver', () => {
  const executeResolver: ResolveFn<StatusCode | null> = (
    ...resolverParameters
  ) =>
    TestBed.runInInjectionContext(() =>
      provincialAttachmentSystemResolver(...resolverParameters),
    );

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeResolver).toBeTruthy();
  });
});
