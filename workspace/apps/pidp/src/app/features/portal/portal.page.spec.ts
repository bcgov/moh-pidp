import { HttpClient } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

import { provideAutoSpy } from 'jest-auto-spies';

import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';

import { PortalResource } from './portal-resource.service';
import { PortalPage } from './portal.page';

describe('PortalPage', () => {
  let component: PortalPage;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [NoopAnimationsModule],
      providers: [
        PortalPage,
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
        provideAutoSpy(HttpClient),
        provideAutoSpy(PortalResource),
      ],
    });
    component = TestBed.inject(PortalPage);
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
