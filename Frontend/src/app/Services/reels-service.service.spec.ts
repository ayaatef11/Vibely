import { TestBed } from '@angular/core/testing';

import { ReelsServiceService } from './reels-service.service';

describe('ReelsServiceService', () => {
  let service: ReelsServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ReelsServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
