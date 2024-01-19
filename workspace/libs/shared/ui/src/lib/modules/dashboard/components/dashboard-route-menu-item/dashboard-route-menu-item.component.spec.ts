/* eslint-disable @typescript-eslint/no-explicit-any */
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute, RouterModule } from '@angular/router';

import { randTextRange } from '@ngneat/falso';

import { DashboardRouteMenuItem } from '../../models/dashboard-menu-item.model';
import { DashboardRouteMenuItemComponent } from './dashboard-route-menu-item.component';

describe('DashboardRouteMenuItemComponent', () => {
  let component: DashboardRouteMenuItemComponent;
  let fixture: ComponentFixture<DashboardRouteMenuItemComponent>;

  let mockActivatedRoute: { snapshot: any };

  beforeEach(async () => {
    mockActivatedRoute = {
      snapshot: {
        data: {
          title: randTextRange({ min: 1, max: 4 }),
          routes: {
            root: '../../',
          },
        },
      },
    };

    await TestBed.configureTestingModule({
      imports: [RouterModule],
      providers: [
        {
          provide: ActivatedRoute,
          useValue: mockActivatedRoute,
        },
      ],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DashboardRouteMenuItemComponent);
    component = fixture.componentInstance;
    component.routeMenuItem = new DashboardRouteMenuItem('FAQ', {
      commands: '/test/faq',
    });
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
