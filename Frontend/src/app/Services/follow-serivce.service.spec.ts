import { TestBed } from '@angular/core/testing';

import { FollowSerivceService } from './follow-serivce.service';

describe('FollowSerivceService', () => {
  let service: FollowSerivceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FollowSerivceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
