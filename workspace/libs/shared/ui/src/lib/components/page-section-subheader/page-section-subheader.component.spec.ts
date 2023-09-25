import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PageSubheaderComponent } from '../page-subheader/page-subheader.component';
import { PageSectionSubheaderComponent } from './page-section-subheader.component';

describe('PageSectionSubheaderComponent', () => {
  let component: PageSectionSubheaderComponent;
  let fixture: ComponentFixture<PageSectionSubheaderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PageSectionSubheaderComponent, PageSubheaderComponent],
    }).compileComponents();
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
