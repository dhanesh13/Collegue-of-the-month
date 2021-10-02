import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VotenowrationaleComponent } from './votenowrationale.component';

describe('VotenowrationaleComponent', () => {
  let component: VotenowrationaleComponent;
  let fixture: ComponentFixture<VotenowrationaleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ VotenowrationaleComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(VotenowrationaleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
