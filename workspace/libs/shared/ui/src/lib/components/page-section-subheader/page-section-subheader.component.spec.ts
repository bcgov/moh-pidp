import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PageSectionSubheaderComponent } from './page-section-subheader.component';

describe('PageSectionSubheaderComponent', () => {
  let component: PageSectionSubheaderComponent;
  let fixture: ComponentFixture<PageSectionSubheaderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({}).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PageSectionSubheaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
