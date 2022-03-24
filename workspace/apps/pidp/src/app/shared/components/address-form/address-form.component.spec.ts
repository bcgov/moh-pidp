import { TestBed } from '@angular/core/testing';

import { provideAutoSpy } from 'jest-auto-spies';

import { FormUtilsService } from '@app/core/services/form-utils.service';
import { LookupService } from '@app/modules/lookup/lookup.service';

import { AddressFormComponent } from './address-form.component';

describe('AddressFormComponent', () => {
  let component: AddressFormComponent;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        AddressFormComponent,
        provideAutoSpy(LookupService),
        provideAutoSpy(FormUtilsService),
      ],
    });

    component = TestBed.inject(AddressFormComponent);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
