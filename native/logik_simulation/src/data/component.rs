use std::fmt::Debug;
use crate::data::subnet::SubnetState;
use crate::map;
use std::collections::HashMap;
use crate::data::EdgeDirection;

/// A trait to define common behaviour between the components
pub(crate) trait Component: Debug {
    fn ports(&self) -> usize;
    fn port_type(&self, port: usize) -> Option<PortType>;
    // requires that data has a value for every input or bidirectional port
    // and in turn guarantees that the return value has a value for every output
    fn evaluate(&self, data: HashMap<usize, SubnetState>) -> Option<HashMap<usize, SubnetState>>;
    
    fn ports_type(&self) -> Vec<PortType> {
        (0..self.ports())
            .map(|e| self.port_type(e).unwrap())
            .collect()
    }
}

#[derive(Debug, Eq, PartialEq, Clone)]
pub(crate) enum PortType {
    Input,
    Output,
    Bidirectional,
}

impl PortType {
    pub(crate) fn to_edge_direction(&self) -> EdgeDirection {
        match self {
            PortType::Input => EdgeDirection::ToComponent,
            PortType::Bidirectional => EdgeDirection::Bidirectional,
            PortType::Output => EdgeDirection::ToSubnet,
        }
    }
}

impl From<PortType> for EdgeDirection {
    fn from(pt: PortType) -> Self {
        match pt {
            PortType::Input => EdgeDirection::ToComponent,
            PortType::Bidirectional => EdgeDirection::Bidirectional,
            PortType::Output => EdgeDirection::ToSubnet,
        }
    }
}

/// Placeholder for now
#[derive(Debug)]
pub(crate) struct Output {

}

impl Component for Output {
    fn ports(&self) -> usize {
        1
    }
    
    fn port_type(&self, port: usize) -> Option<PortType> {
        match port {
            0 => Some(PortType::Input),
            _ => None,
        }
    }
    
    fn evaluate(&self, data: HashMap<usize, SubnetState>) -> Option<HashMap<usize, SubnetState>> {
        Some(map!())
    }
}

#[derive(Debug)]
pub(crate) struct Input {

}

impl Component for Input {
    fn ports(&self) -> usize {
        1
    }
    
    fn port_type(&self, port: usize) -> Option<PortType> {
        match port {
            0 => Some(PortType::Output),
            _ => None,
        }
    }
    
    fn evaluate(&self, data: HashMap<usize, SubnetState>) -> Option<HashMap<usize, SubnetState>> {
        Some(map!(0 => SubnetState::On))
    }
}

/// Placeholder for now
#[derive(Debug)]
pub(crate) struct AND {

}

impl Component for AND {
    fn ports(&self) -> usize {
        3
    }
    
    fn port_type(&self, port: usize) -> Option<PortType> {
        match port {
            0 | 1 => Some(PortType::Input),
            2 => Some(PortType::Output),
            _ => None,
        }
    }
    
    fn evaluate(&self, data: HashMap<usize, SubnetState>) -> Option<HashMap<usize, SubnetState>> {
        if !(data.contains_key(&0) && data.contains_key(&1)) {
            return None;
        }
        
        if matches!(data.get(&0).unwrap(), SubnetState::Error | SubnetState::Floating) ||
            matches!(data.get(&1).unwrap(), SubnetState::Error | SubnetState::Floating) {
            return Some(map!(2 => SubnetState::Error));
        }
        
        if data.get(&0).unwrap() == &SubnetState::On && data.get(&1).unwrap() == &SubnetState::On {
            Some(map!(2 => SubnetState::On))
        } else {
            Some(map!(2 => SubnetState::Off))
        }
    }
}