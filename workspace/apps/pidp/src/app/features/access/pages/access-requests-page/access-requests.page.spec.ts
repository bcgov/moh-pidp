/* eslint-disable @typescript-eslint/no-explicit-any */
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';

import { AccessRequestsPage } from './access-requests.page';

describe('PortalCardComponent', () => {
  let component: AccessRequestsPage;
  let fixture: ComponentFixture<AccessRequestsPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AccessRequestsPage, NoopAnimationsModule],
      providers: [
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
      ],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AccessRequestsPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
