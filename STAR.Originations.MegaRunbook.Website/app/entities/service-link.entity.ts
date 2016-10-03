
import { ApplicationLink } from './application-link.entity';
import { EnvironmentLink } from './environment-link.entity';

export class ServiceLink {

    constructor(
        public Id:                number,
        public ApplicationLinkId: number,
        public ServiceName:       string,

        public EnvironmentLinks:  EnvironmentLink[]
    ) { }

}