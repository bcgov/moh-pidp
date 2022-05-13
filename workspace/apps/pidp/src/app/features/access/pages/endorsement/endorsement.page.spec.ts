import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EndorsementPage } from './endorsement.page';

describe('EndorsementPage', () => {
  let component: EndorsementPage;
  let fixture: ComponentFixture<EndorsementPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EndorsementPage ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EndorsementPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
