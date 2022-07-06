import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EndorsementRequestsReceivedPage } from './endorsement-requests-received.page';

describe('EndorsementRequestsReceivedPage', () => {
  let component: EndorsementRequestsReceivedPage;
  let fixture: ComponentFixture<EndorsementRequestsReceivedPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EndorsementRequestsReceivedPage ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EndorsementRequestsReceivedPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
