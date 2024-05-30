/* eslint-disable @typescript-eslint/no-explicit-any */
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AccessRequestPageComponent } from './access-request-page.component';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { APP_CONFIG, APP_DI_CONFIG } from '@app/app.config';

describe('PortalCardComponent', () => {
  let component: AccessRequestPageComponent;
  let fixture: ComponentFixture<AccessRequestPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AccessRequestPageComponent, NoopAnimationsModule],
      providers: [
        {
          provide: APP_CONFIG,
          useValue: APP_DI_CONFIG,
        },
      ],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AccessRequestPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});