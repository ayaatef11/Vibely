import { TestBed } from '@angular/core/testing';

import { SharePostServiceService } from './share-post-service.service';

describe('SharePostServiceService', () => {
  let service: SharePostServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SharePostServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
