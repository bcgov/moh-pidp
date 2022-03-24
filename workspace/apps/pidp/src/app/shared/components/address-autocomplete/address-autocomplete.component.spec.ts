import { TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';

import { provideAutoSpy } from 'jest-auto-spies';

import { AddressAutocompleteResource } from '@app/core/resources/address-autocomplete-resource.service';
import { ToastService } from '@app/core/services/toast.service';

import { AddressAutocompleteComponent } from './address-autocomplete.component';

describe('AddressAutocompleteComponent', () => {
  let component: AddressAutocompleteComponent;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [ReactiveFormsModule],
      providers: [
        AddressAutocompleteComponent,
        provideAutoSpy(AddressAutocompleteResource),
        provideAutoSpy(ToastService),
      ],
    });

    component = TestBed.inject(AddressAutocompleteComponent);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
