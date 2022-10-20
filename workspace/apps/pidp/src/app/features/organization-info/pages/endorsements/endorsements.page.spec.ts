import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EndorsementsPage } from './endorsements.page';

describe('EndorsementsPage', () => {
  let component: EndorsementsPage;
  let fixture: ComponentFixture<EndorsementsPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EndorsementsPage ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EndorsementsPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
