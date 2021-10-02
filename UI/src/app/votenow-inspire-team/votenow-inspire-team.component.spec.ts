import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VotenowInspireTeamComponent } from './votenow-inspire-team.component';

describe('VotenowInspireTeamComponent', () => {
  let component: VotenowInspireTeamComponent;
  let fixture: ComponentFixture<VotenowInspireTeamComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ VotenowInspireTeamComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(VotenowInspireTeamComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
