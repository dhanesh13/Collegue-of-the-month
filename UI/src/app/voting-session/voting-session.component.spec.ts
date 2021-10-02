import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VotingSessionComponent } from './voting-session.component';

describe('VotingSessionComponent', () => {
  let component: VotingSessionComponent;
  let fixture: ComponentFixture<VotingSessionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ VotingSessionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(VotingSessionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
