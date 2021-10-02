import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VotenowComponent } from './votenow.component';

describe('VotenowComponent', () => {
  let component: VotenowComponent;
  let fixture: ComponentFixture<VotenowComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ VotenowComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(VotenowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
