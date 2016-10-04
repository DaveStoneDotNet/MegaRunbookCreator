
import { Server }      from './server.entity';
import { ServiceLink } from './service-link.entity';

export class EnvironmentLink {

    constructor(
        public Id:            number,
        public ServiceLinkId: number,
        public ServerId:      number,
        public Url:           string, 
        public PathFolder:    string, 
        public IsSelected:    boolean,

        public Server:        Server
    ) { }

}