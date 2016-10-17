
import { ApplicationGroup } from './application-group.entity';
import { ApplicationType }  from './application-type.entity';
import { ServiceLink }      from './service-link.entity';
import { Paging }           from './paging.entity';

export class ApplicationLink {

    Id:                 number;
    ApplicationGroupId: number;
    Name:               string;
    Notes:              string;
    IsSelected:         boolean;

    ApplicationType:    ApplicationType[];
    ServiceLinks:       ServiceLink[];

    Paging:             Paging;
}