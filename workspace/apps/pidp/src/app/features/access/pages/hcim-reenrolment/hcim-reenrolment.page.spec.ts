import { TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';

import { randTextRange } from '@ngneat/falso';
import { createSpyFromClass, provideAutoSpy } from 'jest-auto-spies';

import { PartyService } from '@app/core/party/party.service';
import { FormUtilsService } from '@app/core/services/form-utils.service';
import { LoggerService } from '@app/core/services/logger.service';

import { HcimReenrolmentResource } from './hcim-reenrolment-resource.service';
import { HcimReenrolmentPage } from './hcim-reenrolment.page';

describe('HcimReenrolmentPage', () => {
  let component: HcimReenrolmentPage;

  const mockActivatedRoute = {
    snapshot: {
      data: {
        title: randTextRange({ min: 1, max: 4 }),
        routes: {
          root: '../../',
        },
      },
    },
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule, ReactiveFormsModule, MatDialogModule],
      providers: [
        HcimReenrolmentPage,
        {
          provide: ActivatedRoute,
          useValue: mockActivatedRoute,
        },
        {
          provide: PartyService,
          useValue: createSpyFromClass(PartyService, {
            gettersToSpyOn: ['partyId'],
            settersToSpyOn: ['partyId'],
          }),
        },
        provideAutoSpy(HcimReenrolmentResource),
        provideAutoSpy(FormUtilsService),
        provideAutoSpy(LoggerService),
      ],
    });

    component = TestBed.inject(HcimReenrolmentPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
