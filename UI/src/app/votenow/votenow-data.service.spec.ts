import { TestBed } from '@angular/core/testing';

import { VotenowDataService } from './votenow-data.service';

describe('VotenowDataService', () => {
  let service: VotenowDataService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(VotenowDataService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
