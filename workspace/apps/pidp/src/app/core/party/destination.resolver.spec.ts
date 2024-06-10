import { TestBed } from '@angular/core/testing';
import { ResolveFn } from '@angular/router';

import { destinationResolver } from './destination.resolver';
import { Destination } from './discovery-resource.service';

describe('destinationResolver', () => {
  const executeResolver: ResolveFn<Destination | null> = (
    ...resolverParameters
  ) =>
    TestBed.runInInjectionContext(() =>
      destinationResolver(...resolverParameters),
    );

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeResolver).toBeTruthy();
  });
});
