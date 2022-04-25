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

import { HcimwebAccountTransferResource } from './hcimweb-account-transfer-resource.service';
import { HcimwebAccountTransferPage } from './hcimweb-account-transfer.page';

describe('HcimwebAccountTransferPage', () => {
  let component: HcimwebAccountTransferPage;

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
        HcimwebAccountTransferPage,
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
        provideAutoSpy(HcimwebAccountTransferResource),
        provideAutoSpy(FormUtilsService),
        provideAutoSpy(LoggerService),
      ],
    });

    component = TestBed.inject(HcimwebAccountTransferPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
