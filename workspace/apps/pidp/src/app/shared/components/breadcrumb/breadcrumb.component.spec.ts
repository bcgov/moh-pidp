import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';

import { NavigationService } from '@pidp/presentation';
import { provideAutoSpy } from 'jest-auto-spies';

import { BreadcrumbComponent } from './breadcrumb.component';

describe('BreadcrumbComponent', () => {
  let component: BreadcrumbComponent;
  let fixture: ComponentFixture<BreadcrumbComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BreadcrumbComponent],
      providers: [provideAutoSpy(NavigationService), provideAutoSpy(Router)],
    }).compileComponents();

    fixture = TestBed.createComponent(BreadcrumbComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
