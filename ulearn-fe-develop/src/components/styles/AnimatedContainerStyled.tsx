import { motion } from "framer-motion"
import { ContainerStyled } from "./ContainerStyled";


export const AnimatedContainer = ({ children }: { children: any }) => (
    <motion.div
        initial={{ opacity: 0 }}
        whileInView={{ opacity: 1 }}
        transition={{ duration: 0.5, delay: 0.5 }}
        viewport={{ once: true }}
    >
        <ContainerStyled>
            {children}
        </ContainerStyled>
    </motion.div>
)
