import { TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';

import { provideAutoSpy } from 'jest-auto-spies';

import { FormUtilsService } from './form-utils.service';
import { LoggerService } from './logger.service';

describe('FormUtilsService', () => {
  let service: FormUtilsService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [ReactiveFormsModule],
      providers: [provideAutoSpy(LoggerService)],
    });
    service = TestBed.inject(FormUtilsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
