import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InspireTeamComponent } from './inspire-team.component';

describe('InspireTeamComponent', () => {
  let component: InspireTeamComponent;
  let fixture: ComponentFixture<InspireTeamComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InspireTeamComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InspireTeamComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
