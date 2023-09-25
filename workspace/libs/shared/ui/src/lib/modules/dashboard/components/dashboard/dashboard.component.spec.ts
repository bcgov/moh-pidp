import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MatListModule } from '@angular/material/list';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';

import { BcGovLogoComponent } from '../../../../components/bc-gov-logo/bc-gov-logo.component';
import { NgxProgressBarModule } from '../../../../modules/ngx-progress-bar/ngx-progress-bar.module';
import { DashboardHeaderComponent } from '../dashboard-header/dashboard-header.component';
import { DashboardMenuComponent } from '../dashboard-menu/dashboard-menu.component';
import { DashboardComponent } from './dashboard.component';

describe('DashboardComponent', () => {
  let component: DashboardComponent;
  let fixture: ComponentFixture<DashboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [
        BcGovLogoComponent,
        DashboardComponent,
        DashboardHeaderComponent,
        DashboardMenuComponent,
      ],
      imports: [
        BrowserAnimationsModule,
        MatListModule,
        MatSidenavModule,
        MatToolbarModule,
        NgxProgressBarModule,
        RouterModule,
      ],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
